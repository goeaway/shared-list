import React, { FC, useState, useLayoutEffect, useEffect, useRef, MutableRefObject } from "react";
import styled, {css} from "styled-components";
import { FaPlus } from "react-icons/fa";
import useKeyPress from "../hooks/use-key-press";
import { AccentButton } from "./style/buttons";
import { Input } from "./style/inputs";

export interface ListInputProps {
    onAdd: (value: string) => void;
    focus?: boolean;
    clickOutsideRef: MutableRefObject<HTMLInputElement>;
    onLoseFocus: () => void;
    onRequestFocus: () => void;
}

const ListInput : FC<ListInputProps> = ({ onAdd, focus, clickOutsideRef, onLoseFocus, onRequestFocus }) => {
    const [value, setValue] = useState("");
    const inputRef = useRef<HTMLInputElement>(null);

    const enter = useKeyPress("Enter");
    const escape = useKeyPress("Escape");

    useEffect(() => {
        if(focus) {
            buttonClickHandler();
        }
    }, [enter]);

    useEffect(() => {
        if(focus) {
            inputRef.current.blur();
            onLoseFocus();
        }
    }, [escape]);

    useEffect(() => {
        if(focus) {
            inputRef.current.focus();
        } else {
            inputRef.current.blur();
        }
    }, [focus]);

    const valueChangeHandler = (event: React.ChangeEvent<HTMLInputElement>) => {
        setValue(event.target.value);
    }

    const buttonClickHandler = () => {
        if(value) {
            onAdd(value);
            setValue("");
        }
    }

    return (
        <StyledContainer ref={clickOutsideRef} role="list-input">
            <Input withAddon ref={inputRef} type="text" value={value} placeholder="Add an item..." onClick={onRequestFocus} onChange={valueChangeHandler} />
            <AccentButton append role="add-item-button" onClick={buttonClickHandler}><FaPlus /></AccentButton>
        </StyledContainer>
    );
}

export default ListInput;

const StyledContainer = styled.div`
    display:flex;
`