import React, { FC } from "react";
import styled from "styled-components";
import { ListDTO } from "../types";
import { useHistory } from "react-router";
import { FaPlus, FaTrash } from "react-icons/fa";
import { Link } from "react-router-dom";
import { AccentButton, IconButton } from "./style/buttons";
import useAuth from "../hooks/use-auth";
import ListEmpty from "./list-empty";

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
                    lists.map(l => 
                        <ListPreview key={l.id}>
                            <ListPreviewLink to={`/list/${l.id}`} >
                                {l.name}
                            </ListPreviewLink>
                            <DeleteButton onClick={() => onDelete(l.id)}>
                                <FaTrash />
                            </DeleteButton>
                        </ListPreview>)
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

const EmptyState = styled.div`

`

const ListsContainer = styled.div`
    display: flex;
    flex-direction: column;
    padding: .25rem 0;
`

const ListPreview = styled.div`
    width: 100%;
    border: 2px solid ${p => p.theme.gray1};
    border-radius: 3px;
    transition: all 300ms ease;
    display: flex;
    justify-content: space-between;
    align-items: center;
    color: ${p => p.theme.fontLight5};
    margin: .25rem 0;
    position: relative;

    &:hover {
        background: ${p => p.theme.background2};
    }
`
    
const ListPreviewLink = styled(Link)`
    text-decoration: none;
    padding: .75rem;
    color: ${p => p.theme.fontLight5};
    flex: 1 1 auto;

    &:hover {
        color: ${p => p.theme.fontDark2};
    }
`

const DeleteButton = styled(IconButton)`
    padding: .75rem;
    position: absolute;
    right: .5rem;
`