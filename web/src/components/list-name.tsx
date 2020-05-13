import React, {FC, useState, useEffect, useRef} from "react";
import styled from "styled-components";
import { Input, SubtleInput } from "./style/inputs";
import { FaCheck } from "react-icons/fa";
import { AccentButton } from "./style/buttons";
import useClickOutside from "../hooks/use-click-outside";
import useKeyPress from "../hooks/use-key-press";

export interface ListNameProps {
    name: string;
    onChange: (value: string) => void;
    onEditChanged: (editing: boolean) => void;
}

const ListName : FC<ListNameProps> = ({ name, onChange, onEditChanged }) => {
    const [editing, setEditing] = useState(false);
    const [editingValue, setEditingValue] = useState("");
    
    const inputRef = useRef<HTMLInputElement>(null);

    const enter = useKeyPress("Enter");
    const escape = useKeyPress("Escape");

    const confirmHandler = () => {
        if(editing) {
            setEditing(false);
            onChange(editingValue || name);
        }
    }

    const clickOutsideRef = useClickOutside(confirmHandler);

    useEffect(confirmHandler, [enter]);

    useEffect(() => {
        if(editing) {
            setEditing(false);
        }
    }, [escape]);

    useEffect(() => {
        setEditingValue(name);
    }, [name]);

    useEffect(() => {
        onEditChanged(editing);

        if(editing) {
            inputRef.current.focus();
        }
    }, [editing]);

    const valueChangedHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setEditingValue(event.target.value);
    }

    const spanClickHandler = () => {
        setEditing(true);
    }

    return (
        <StyledContainer role="list-name" ref={clickOutsideRef}>
            {!editing && <NameSpan role="list-name-span" onClick={spanClickHandler}>{name}</NameSpan>}
            {editing && <SubtleInput value={editingValue} role="list-name-input" ref={inputRef} onChange={valueChangedHandler} />}
        </StyledContainer>
    );
}

export default ListName;

const StyledContainer = styled.div`
    display:flex;
    font-size: 22px;
`

const NameSpan = styled.span`
    padding: .25rem;
    padding-left: 0;
    border: 2px solid transparent;
`