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
    
    const enter = useKeyPress("Enter");
    const escape = useKeyPress("Escape");

    const onEditConfirmButtonHandler = () => {
        if(editing) {
            onItemEdited(listItem.id, { id: listItem.id, notes: listItem.notes, value: editedValue || listItem.value, complete: listItem.complete });
            setEditing(false);
        }
    }

    const outsideClickRef = useClickOutside(onEditConfirmButtonHandler);
    
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
        onItemEdited(listItem.id, { id: listItem.id, notes: listItem.notes, value: listItem.value, complete: value });
    }

    const onContainerClickHandler = () => {
        setEditing(true);
    }

    const inputChangedHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setEditedValue(event.target.value);
    }

    const contextMenuDeleteClickHandler = () => {
        onDelete(listItem.id);
    }

    return (
        <OuterContainer>
            <Draggable draggableId={listItem.id + ""} index={index}>
                {(provided, snapshot) => (
                    <InnerContainer 
                        {...provided.draggableProps}
                        ref={provided.innerRef}
                        isDragging={snapshot.isDragging}
                        shouldHide={hide && listItem.complete}
                        role="list-item">
                        <ContentContainer ref={outsideClickRef}>
                            <ContextMenuTrigger id={`list-item-context-${listItem.id}`}>
                                <ListCheckbox value={listItem.complete} onChange={checkboxChangedHandler} />
                                <Spacer marginX={4} />
                                {!editing && <StyledSpan role="list-item-value" onClick={onContainerClickHandler} complete={listItem.complete}>{listItem.value}</StyledSpan>}            
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
        </OuterContainer>
    );
}

export default ListItem;

const OuterContainer = styled.div`
    
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
    text-decoration: ${(p: StyledSpanProps) => p.complete && "line-through"};
    margin-left: .5rem;
    flex: 1 1 auto;
    padding: .25rem;
    border: 2px solid transparent;
`