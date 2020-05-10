import React, { FC } from "react";
import styled from "styled-components";
import { FaClipboard } from "react-icons/fa";

const ListEmpty : FC = () => {
    return (
        <StyledContainer role="list-empty">
            <FaClipboard role="list-empty-icon" size={100} />
            <Text role="list-empty-text">No items, press Space to start</Text>
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