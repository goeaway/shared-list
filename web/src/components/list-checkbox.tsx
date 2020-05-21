import React, { FC } from "react";
import { FaRegCircle, FaCheckCircle } from "react-icons/fa";
import styled, { css } from "styled-components";

export interface ListCheckboxProps {
    value: boolean;
    onChange: (value: boolean) => void;
}

const ListCheckbox: FC<ListCheckboxProps> = ({value, onChange}) => {
    const onClickHandler = () => {
        onChange(!value);
    }

    return (
        <StyledContainer onClick={onClickHandler} checked={value} role="list-item-check">
            {value ? <FaCheckCircle role="list-item-check-complete" size={20} /> : <FaRegCircle role="list-item-check-incomplete" size={20} />}
        </StyledContainer>
    );
}

export default ListCheckbox;

interface StyledContainerProps {
    checked: boolean;
}

const StyledContainer = styled.div`
    display:flex;
    align-items: center;
    color: ${p => p.theme.gray5};
    transition: color .3s ease;
    cursor: pointer;

    ${(p: StyledContainerProps) => p.checked && css`
        color: ${p => p.theme.accent4};
    `}
`