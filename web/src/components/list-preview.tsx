import React, { FC, useState } from "react";
import { ListDTO } from "../types";
import styled from "styled-components";
import InfoTooltip from "./tooltips/info-tooltip";
import { Link } from "react-router-dom";
import { IconButton } from "./style/buttons";
import { FaTrash } from "react-icons/fa";
import { CSSTransition } from "react-transition-group";
import ConfirmTooltip from "./tooltips/confirm-tooltip";

export interface ListPreviewProps {
    list: ListDTO;
    onDelete: (id: string) => void;
}

const ListPreview : FC<ListPreviewProps> = ({list, onDelete}) => {
    const [transIn, setTransIn] = useState(true);
    const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);

    const onDeleteHandler = () => {
        setShowDeleteConfirm(true);
    }
    
    const deleteConfirmHandler = () => {
        setShowDeleteConfirm(false);
        setTransIn(false);
    
        setTimeout(() => {
            onDelete(list.id);
        }, 300);    
    }

    const deleteCancelHandler = () => {
        setShowDeleteConfirm(false);
    }

    return (
        <Container>
            <CSSTransition in={transIn} timeout={300} classNames="list-preview">
                <InnerContainer>
                    <ListPreviewLink to={`/list/${list.id}`} >
                        {list.name}
                    </ListPreviewLink>
                    <ConfirmTooltip position="left" content={<span>Are you sure you want to delete this list?</span>} onConfirm={deleteConfirmHandler} onDismiss={deleteCancelHandler} show={showDeleteConfirm}>
                        <InfoTooltip position="left" content={<span>Delete List</span>} disable={showDeleteConfirm}>
                            <DeleteButton onClick={onDeleteHandler}>
                                <FaTrash />
                            </DeleteButton>
                        </InfoTooltip>
                    </ConfirmTooltip>
                </InnerContainer>
            </CSSTransition>
        </Container>
    );
}

export default ListPreview;

const Container = styled.div`
    .list-preview-enter {
        opacity: 0;
        position: relative;
        top: 10px;
    }

    .list-preview-enter-active {
        opacity: 1;
        top: 0;
        position: relative;
        transition: opacity 300ms, top 300ms;
    }

    .list-preview-exit {
        opacity: 1;
        top: 0;
        left: 0;
        position: relative;
    }

    .list-preview-exit-active {
        opacity: 0;
        left: -100px;
        position: relative;
        transition: opacity 300ms, left 300ms;
    }

    .list-preview-exit-done {
        opacity: 0;
        left: -100px;
        position: relative;
    }
`

const InnerContainer = styled.div`
    border: 2px solid ${p => p.theme.gray1};
    border-radius: 3px;
    color: ${p => p.theme.fontLight5};
    margin: .25rem 0;
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    transition: border-color 300ms;

    &:hover {
        border-color: ${p => p.theme.gray4};
    }
`
    
const ListPreviewLink = styled(Link)`
    text-decoration: none;
    padding: .75rem;
    color: ${p => p.theme.fontLight5};
    flex: 1 1 auto;
    transition: color 300ms;

    &:hover {
        color: ${p => p.theme.fontDark2};
    }
`


const DeleteButton = styled(IconButton)`
    margin-right: .5rem;
`