import React, { FC, useState } from "react";
import styled from "styled-components";
import { ListPreviewDTO } from "../types";
import { FaPlus, FaSpinner } from "react-icons/fa";
import { AccentButton } from "./style/buttons";
import ListEmpty from "./list-empty";
import ListPreview from "./list-preview";
import { config } from "@config/production";

export interface ListListProps {
    lists: Array<ListPreviewDTO>;
    onDelete: (id: string) => void;
    onAdd: () => void;
    adding?: boolean;
    loading?: boolean;
}

const ListList : FC<ListListProps> = ({ lists, onDelete, onAdd, adding, loading }) => {
    return (
        <Container>
            <ControlBar>
                <Title>Your Lists</Title>            
                <AccentButton disabled={lists.length >= config.limits.lists || adding} onClick={onAdd}>{adding ? <FaSpinner className="fa-spin" /> : <FaPlus />}&nbsp;Add New List</AccentButton>
            </ControlBar> 
            {
                <ListsContainer>
                    {lists.length === 0 && (loading ? <ListEmpty text="Loading lists..." icon={FaSpinner} spin /> : <ListEmpty text="No lists yet..."></ListEmpty>)}
                    {
                        lists.length > 0 && 
                        lists.map(l => <ListPreview key={l.id} list={l} onDelete={onDelete} />)
                    }
                </ListsContainer>
            }
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
