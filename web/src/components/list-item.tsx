import React, { FC, useState, useLayoutEffect, useEffect, useRef } from "react";
import { ListItemDTO } from "../types";
import styled, {css} from "styled-components";
import { SubtleInput } from "./style/inputs";
import { FaGripVertical } from "react-icons/fa";
import useClickOutside from "../hooks/use-click-outside";
import useKeyPress from "../hooks/use-key-press";
import { ContextMenu, MenuItem, ContextMenuTrigger } from "react-contextmenu";
import { Draggable } from "react-beautiful-dnd";
import ListCheckbox from "./list-checkbox";
import { Spacer } from "./style/utilities";
import { CSSTransition } from "react-transition-group";

export interface ListItemProps {
    listItem: ListItemDTO;
    index: number;
    onItemEdited: (id: string, item: ListItemDTO) => void;
    onDelete: (id: string) => void;
    onEditingChanged: (editing: boolean) => void;
    hide?: boolean;
}

const ListItem : FC<ListItemProps> = ({index, listItem, onItemEdited, onEditingChanged, onDelete, hide }) => {
    const [editing, setEditing] = useState(false);
    const [editedValue, setEditedValue] = useState("");
    const inputRef = useRef<HTMLInputElement>(null);
    const [transitionIn, setTransitionIn] = useState(false);
    
    const enter = useKeyPress("Enter");
    const escape = useKeyPress("Escape");

    const onEditConfirmButtonHandler = () => {
        if(editing) {
            onItemEdited(listItem.id, { id: listItem.id, notes: listItem.notes, value: editedValue || listItem.value, completed: listItem.completed });
            setEditing(false);
        }
    }

    const outsideClickRef = useClickOutside(onEditConfirmButtonHandler);

    useEffect(() => {
        setTransitionIn(true);
    }, []);
    
    useEffect(() => {
        if(editing) {
            onEditConfirmButtonHandler();
        }
    }, [enter]);

    useEffect(() => {
        if(editing) {
            setEditing(false);
        }
    }, [escape]);

    useLayoutEffect(() => {
        setEditedValue(listItem.value);
    }, [listItem.value]);

    useEffect(() => {
        if(editing) {
            inputRef.current.focus();  
        }  
        onEditingChanged(editing);
    }, [editing]);

    const checkboxChangedHandler = (value: boolean) => {
        onItemEdited(listItem.id, { id: listItem.id, notes: listItem.notes, value: listItem.value, completed: value });
    }
    
    const onContainerClickHandler = () => {
        setEditing(true);
    }
    
    const inputChangedHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setEditedValue(event.target.value);
    }
    
    const contextMenuDeleteClickHandler = () => {
        setTransitionIn(false);

        setTimeout(() => {
            onDelete(listItem.id);
        }, 300);
    }
    
    return (
        <OuterContainer>
            <CSSTransition in={transitionIn} timeout={300} classNames="show-list-item">
                <Draggable draggableId={listItem.id + ""} index={index}>
                    {(provided, snapshot) => (
                        <InnerContainer 
                        {...provided.draggableProps}
                        ref={provided.innerRef}
                        isDragging={snapshot.isDragging}
                        shouldHide={hide && listItem.completed}
                        role="list-item">
                            <ContentContainer ref={outsideClickRef}>
                                <ContextMenuTrigger id={`list-item-context-${listItem.id}`}>
                                    <ListCheckbox value={listItem.completed} onChange={checkboxChangedHandler} />
                                    <Spacer marginX={4} />
                                    {!editing && <StyledSpan role="list-item-value" onClick={onContainerClickHandler} complete={listItem.completed}>{listItem.value}</StyledSpan>}            
                                    {editing && <SubtleInput ref={inputRef} role="list-item-input" value={editedValue} onChange={inputChangedHandler}></SubtleInput>}
                                </ContextMenuTrigger>
                            </ContentContainer>
                            <DragHandle {...provided.dragHandleProps}><FaGripVertical /></DragHandle>
                            <ContextMenu id={`list-item-context-${listItem.id}`}>
                                <MenuItem onClick={contextMenuDeleteClickHandler}>Delete</MenuItem>
                            </ContextMenu>
                        </InnerContainer>
                    )}
                </Draggable>
            </CSSTransition>
        </OuterContainer>
    );
}

export default ListItem;

const OuterContainer = styled.div`
    .show-list-item-enter {
        opacity: 0;
        position: relative;
        top: 10px;
    }

    .show-list-item-enter-active {
        opacity: 1;
        top: 0;
        position: relative;
        transition: opacity 300ms, top 300ms;
    }

    .show-list-item-exit {
        opacity: 1;
        position: relative;
        top: 0;
        left: 0;
    }

    .show-list-item-exit-active {
        opacity: 0;
        left: -100px;
        position: relative;
        transition: opacity 300ms, left 300ms;
    }
`

interface InnerContainerStyleProps {
    isDragging: boolean;
    shouldHide: boolean;
}

const InnerContainer = styled.div`
    border-radius: 3px;
    border: 2px solid ${p => p.theme.gray1};
    margin: .5rem 0;
    transition: box-shadow: .3s ease;
    display: flex;
    align-items: center;
    background: white;
    overflow: hidden;
    text-overflow: ellipsis;
    background: ${p => p.theme.background1};

    ${(p: InnerContainerStyleProps) => p.isDragging && css`
        box-shadow:
            0 1.1px 0.9px -6px rgba(0, 0, 0, 0.028),
            0 3.6px 3.1px -6px rgba(0, 0, 0, 0.042),
            0 16px 14px -6px rgba(0, 0, 0, 0.07);
    `}

    ${(p: InnerContainerStyleProps) => p.shouldHide && css`
        display:none;
    `}
`

const ContentContainer = styled.div`
    padding: .5rem;
    flex: 1 1 auto;
    display:flex;
    align-items: center;
`

const DragHandle = styled.div`
    display:flex;
    padding: .5rem;
`

interface StyledSpanProps {
    complete: boolean;
}

const StyledSpan = styled.span`
    margin-left: .5rem;
    flex: 1 1 auto;
    padding: .25rem;
    border: 2px solid transparent;
    position: relative;

    ${(p: StyledSpanProps) => p.complete && css`
        &::after {
            content: ' ';
            position: absolute;
            top: 50%;
            left: 0;
            width: 100%;
            height: 1px;
            background: ${p => p.theme.fontLight5};
            animation-name: strike;
            animation-duration: 100ms;
            animation-timing-function: linear;
            animation-iteration-count: 1;
            animation-fill-mode: forwards; 
        }
    `}

    @keyframes strike{
        0%   { width : 0; }
        100% { width: 100%; }
    }
`