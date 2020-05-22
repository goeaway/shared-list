import React, { FC } from "react";
import styled from "styled-components";
import { ListDTO } from "../types";
import { useHistory } from "react-router";
import { FaPlus } from "react-icons/fa";
import { AccentButton } from "./style/buttons";
import ListEmpty from "./list-empty";
import ListPreview from "./list-preview";

export interface ListListProps {
    lists: Array<ListDTO>;
    onDelete: (id: string) => void;
}

const ListList : FC<ListListProps> = ({ lists, onDelete }) => {
    const { push } = useHistory();
    const onNewListClickHandler = () => {
        push("/list");
    }

    return (
        <Container>
            <ControlBar>
                <Title>Your Lists</Title>            
                <AccentButton onClick={onNewListClickHandler}><FaPlus />&nbsp;Add New List</AccentButton>
            </ControlBar>
            <ListsContainer>
                {lists.length === 0 && <ListEmpty text="No lists yet..."></ListEmpty>}
                {
                    lists.length > 0 && 
                    lists.map(l => <ListPreview key={l.id} list={l} onDelete={onDelete} />)
                }
            </ListsContainer>
        </Container>
    );
}

export default ListList;

const Container = styled.div`

`

const ControlBar = styled.div`
    display: flex;
    justify-content: space-between;
    align-items: center;
`

const Title = styled.span`
    font-size: 22px;
    line-height: 34px;
`

const ListsContainer = styled.div`
    display: flex;
    flex-direction: column;
    padding: .25rem 0;
`
