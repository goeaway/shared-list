import React, { FC } from "react";
import styled from "styled-components";
import { FaTasks } from "react-icons/fa";
import { IconType } from "react-icons/lib/cjs/iconBase";

export interface ListEmptyProps {
    text?: string;
    icon?: IconType;
    spin?: boolean;
}

const ListEmpty : FC<ListEmptyProps> = ({ text, icon: Icon, spin }) => {
    return (
        <StyledContainer role="list-empty">
            {Icon ? <Icon role="list-empty-icon" size={100} className={spin ? "fa-spin" : ""} /> : <FaTasks role="list-empty-icon" size={100} className={spin ? "fa-spin" : ""} />}
            <Text role="list-empty-text">{text || "No items, press Space to start"}</Text>
        </StyledContainer>
    );
}

export default ListEmpty;

const StyledContainer = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 2rem;
    flex-direction: column;
    color: ${p => p.theme.background3};

    > *:first-child {
        margin-bottom: 1rem;
    }
`
    
const Text = styled.span`
    font-size: 14px;
    color: ${p => p.theme.fontLight4};
`