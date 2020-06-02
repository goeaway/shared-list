import React, { FC } from "react";
import { FaRegCircle, FaCheckCircle } from "react-icons/fa";
import styled, { css } from "styled-components";
import { CSSTransition } from "react-transition-group";

export interface ListCheckboxProps {
    value: boolean;
    onChange: (value: boolean) => void;
}

const ListCheckbox: FC<ListCheckboxProps> = ({value, onChange}) => {
    const onClickHandler = () => {
        onChange(!value);
    }

    return (
        <StyledButton onClick={onClickHandler} checked={value} type="button" role="list-item-check">
            {value ? <CSSTransition timeout={300} classNames="screw-in" in appear><FaCheckCircle role="list-item-check-complete" size={20} /></CSSTransition> : <FaRegCircle role="list-item-check-incomplete" size={20} />}
        </StyledButton>
    );
}

export default ListCheckbox;

interface StyledContainerProps {
    checked: boolean;
}

const StyledButton = styled.button`
    display:flex;
    align-items: center;
    color: ${p => p.theme.gray5};
    transition: all .3s ease;
    cursor: pointer;
    outline: none;
    background: none;
    border: none;
    padding: 0;

    &:hover {
        transform: scale(1.1);
    }

    ${(p: StyledContainerProps) => p.checked && css`
        color: ${p => p.theme.accent4};

        &:hover {
            color: ${p => p.theme.accent5};
        }
    `}

    .screw-in-appear {
        transform: rotate(-90deg);
    }

    .screw-in-appear-active {
        transform: rotate(0);
        transition: transform 300ms ease;
    }
    
    .screw-in-appear-done {
        transform: rotate(0);
    }
`