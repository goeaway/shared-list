import React, { FC } from "react";
import styled from "styled-components";

export interface ListItemDetailsProps {

}

const ListItemDetails : FC<ListItemDetailsProps> = ({}) => {
    return (
        <StyledContainer role="list-item-detail">
        </StyledContainer>
    );
}

export default ListItemDetails;

const StyledContainer = styled.div`

`