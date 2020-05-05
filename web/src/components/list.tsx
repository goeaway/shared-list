import React, { FC, useState, useEffect } from "react";
import { ListDTO, ListItemDTO } from "../types";
import ListInput from "./list-input";
import { AccentButton, DefaultButton } from "./style/buttons";
import ListName from "./list-name";
import ListEmpty from "./list-empty";
import ListItem from "./list-item";
import { v1 } from "uuid";
import useKeyPress from "../hooks/use-key-press";
import useClickOutside from "../hooks/use-click-outside";
import { FaShareAlt, FaUndoAlt, FaEye, FaEyeSlash } from "react-icons/fa";
import styled from "styled-components";
import { DragDropContext, Droppable, DropResult } from "react-beautiful-dnd";
import { useToasts } from "react-toast-notifications";

export interface ListProps {
    list: ListDTO;
    canCopy: boolean;
    onChange?: (newList: ListDTO) => void;
}

const List : FC<ListProps> = ({ list, canCopy, onChange }) => {
    const [items, setItems] = useState(list.items);
    const [stopListKeyboardControls, setStopListKeyboardControls] = useState(false);
    const [inputFocus, setInputFocus] = useState(true);
    const [listName, setListName] = useState(list.name);
    const [hideCompleted, setHideCompleted] = useState(false);
    const { addToast } = useToasts();

    const outsideInputClickRef = useClickOutside(() => setInputFocus(false));
    const space = useKeyPress(" ");

    useEffect(() => {
        setListName(list.name);
    }, [list.name]);

    useEffect(() => {
        setItems(list.items);
    }, [list.items]);

    useEffect(() => {
        if(!stopListKeyboardControls) {
            setInputFocus(true);
        }
    }, [space]);

    useEffect(() => {
        if(items !== list.items) {
            onChange({ id: list.id, name: listName, items });
        }
    }, [items]);

    useEffect(() => {
        if(listName !== list.name) {
            onChange({ id: list.id, name: listName, items });
        }
    }, [listName]);

    const inputOnAddHandler = (value: string) => {
        setItems(i => [...i, { id: v1(), value, notes: "", completed: false }]);
    }

    const updateItemHandler = (id: string, item: ListItemDTO) => {
        setItems(i => {
            const copy = [...i];
            const existing = copy.findIndex(c => c.id === id);
            copy.splice(existing, 1, item);
            return copy;
        });
    }

    const itemEditingChangedHandler = (editing: boolean) => {
        setStopListKeyboardControls(editing);
    }

    const itemDeleteHandler = (id: string) => {
        setItems(i => {
            const copy = [...i];
            const existing = copy.findIndex(c => c.id === id);
            copy.splice(existing, 1);
            return copy;
        });
    }

    const inputRequestLoseFocusHandler = () => {
        setInputFocus(false);
    }

    const inputRequestFocusHandler = () => {
        setInputFocus(true);
    }

    const listNameOnChangedHandler = (value: string) => {
        setListName(value);
    }

    const dragEndHandler = (result: DropResult) => {
        const { destination, source } = result;

        if(!destination || (destination.droppableId === source.droppableId && destination.index === source.index)) {
            return;
        }

        setItems(i => {
            const copy = [...i];
            const sourceItem = copy[source.index];
            copy.splice(source.index, 1);
            copy.splice(destination.index, 0, sourceItem);
            return copy;
        });
    }

    const onToggleHideCompleteHandler = () => {
        setHideCompleted(!hideCompleted);
    }

    const onCopyURLClickHandler = async () => {
        await navigator.clipboard.writeText(location.href);
        // trigger toast
        addToast(<span>URL copied to clipboard, share with others to collaborate on this list!</span>, {
            appearance: "info",
            autoDismiss: true
        });
    }

    const onUnCompleteAllClickHandler = () => {
        setItems(i => {
            const copy = [...i];
            copy.forEach(c => c.completed = false);
            return copy;
        });

        addToast(<span>Reset all items to incomplete.</span>, {
            appearance: "info",
            autoDismiss: true
        });
    }

    return (
        <StyledContainer role="list">
            <ControlBar role="list-control-bar">
                <ControlBarInner>
                    <AccentButton hideTextXS disabled={!canCopy} role="copy-url" onClick={onCopyURLClickHandler}><FaShareAlt /><span>&nbsp;Share List</span></AccentButton>
                </ControlBarInner>
                <ControlBarInner>
                    <DefaultButton hideTextXS role="reset-completed" onClick={onUnCompleteAllClickHandler}><FaUndoAlt /><span>&nbsp;Uncomplete All</span></DefaultButton>
                    <DefaultButton hideTextXS role="hide-completed" onClick={onToggleHideCompleteHandler}>{hideCompleted ? <><FaEye /><span>&nbsp;Show Completed</span></> : <><FaEyeSlash /><span>&nbsp;Hide Completed</span></>}</DefaultButton>
                </ControlBarInner>
            </ControlBar>
            <ListName name={listName} onChange={listNameOnChangedHandler} onEditChanged={itemEditingChangedHandler} />
            <ListInput onAdd={inputOnAddHandler} onLoseFocus={inputRequestLoseFocusHandler} onRequestFocus={inputRequestFocusHandler} focus={inputFocus} clickOutsideRef={outsideInputClickRef} />
            <ItemContainer role="list-items-container">
                {items.length === 0 && <ListEmpty />}
                {items.length > 0 && 
                    <DragDropContext onDragEnd={dragEndHandler}>
                        <Droppable droppableId="list-item-drop">
                            {provided => (
                                <div {...provided.droppableProps} ref={provided.innerRef}>
                                    {items.map((item, index) => <ListItem 
                                        key={item.id} 
                                        listItem={item} 
                                        index={index} 
                                        hide={hideCompleted}
                                        onEditingChanged={itemEditingChangedHandler} 
                                        onItemEdited={updateItemHandler}
                                        onDelete={itemDeleteHandler}/>)}
                                    {provided.placeholder}
                                </div>
                            )}
                        </Droppable>
                    </DragDropContext>
                }
            </ItemContainer>
        </StyledContainer>
    );
}

export default List;

const StyledContainer = styled.div`
    > * {
        margin: .5rem 0;
    }

    .react-contextmenu {
        border-radius: 3px;
        box-shadow: 0 0px 5px rgba(0, 0, 0, 0.07);
        border: 1px solid #ddd;
        width: 120px;
        font-size: 14px;
    }

    .react-contextmenu-wrapper {
        flex: 1 1 auto;
        display:flex;
    }

    .react-contextmenu-item {
        padding: .5rem;
        cursor: pointer;
        background: ${p => p.theme.background1};
        transition: all .1s ease;
        outline: none;
    }

    .react-contextmenu-item--selected {
        background: ${p => p.theme.background2};
        color: ${p => p.theme.fontDark1};
    }
`

const ControlBar = styled.div`
    display:flex;
    justify-content: space-between;
`

const ControlBarInner = styled.div`
    display:flex;
    justify-content: space-around;

    > * {
       margin: 0 .5rem; 
    }

    > *:first-child {
        margin-left: 0;
    }

    > *:last-child {
        margin-right: 0;
    }
`

const ItemContainer = styled.div`
    transition: width .3s ease;
`