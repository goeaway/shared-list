import React, { FC, useState, useEffect } from "react";
import { NameAndId } from "../types";
import { FaBars, FaTimes } from "react-icons/fa";
import styled, { css } from "styled-components";
import { InvertedAccentButton } from "./style/buttons";
import useClickOutside from "../hooks/use-click-outside";
import { CSSTransition } from "react-transition-group";
import { useHistory } from "react-router-dom";

export interface ListMenuProps {
    lists: Array<NameAndId<string>>;
}

const ListMenu : FC<ListMenuProps> = ({lists}) => {
    const [open, setOpen] = useState(false);
    const { push } = useHistory();

    const clickOutside = useClickOutside(() => open && setOpen(false));

    const expandButtonClickHandler = () => {
        setOpen(true);
    }

    const onLinkClick = (id: string = "") => {
        setOpen(false);

        setTimeout(() => {
            push("/" + id);
        }, 300);
    }

    return (
        <MenuContainer ref={clickOutside}>
            <ExpandButtonContainer hide={open}>
                <InvertedAccentButton onClick={expandButtonClickHandler}>
                    <FaBars size={25}/>
                </InvertedAccentButton>
            </ExpandButtonContainer>
            <CSSTransition in={open} timeout={300} classNames="show-menu">
                <Menu>
                    <MenuList>
                        <MenuItem onClick={() => onLinkClick()}>
                            <ListName>New List</ListName>
                            <ListLink>/</ListLink>
                        </MenuItem>
                        {lists.map(l => <MenuItem onClick={() => onLinkClick(l.id)} key={l.id}>
                            <ListName>{l.name}</ListName>
                            <ListLink>{"/" + l.id}</ListLink>
                        </MenuItem>)}
                    </MenuList>
                </Menu>
            </CSSTransition>
        </MenuContainer>
    ); 
}

export default ListMenu;

interface ExpandButtonContainerProps {
    hide: boolean;
}

const ExpandButtonContainer = styled.div`
    padding: .5rem;
    position: absolute;
    transition: opacity 600ms;
    ${(p: ExpandButtonContainerProps) => p.hide && css`
        opacity: 0;
    `}
`

const MenuContainer = styled.div`
    position: relative;
    height: 100vh;
    

    .show-menu-enter {
        width: 0;
    }

    .show-menu-enter-active {
        width: 200px;
        transition: width 300ms;
    }

    .show-menu-enter-done {
        width: 200px;
    }

    .show-menu-exit {
        width: 200px;
    }

    .show-menu-exit-active {
        width: 0;
        transition: width 300ms;
    }

    .show-menu-exit-done {
        width: 0;
    }
`

const MenuList = styled.ul`
    padding: 0;
    margin: 0;
    list-style: none;
    display: inline-block;
    width: 200px;
    
    height: 100%;
`

const MenuItem = styled.li`
    display: flex;
    flex-direction: column;
    padding: 1rem;
    transition: background 300ms ease;
    cursor: pointer;
    text-decoration: none;
    
    &:hover > * {
        color: ${p => p.theme.fontDark1};
    }
    
    & > *:first-child {
        margin-bottom: .25rem;
    }
    
    &:hover {
        background: ${p => p.theme.background2};
    }
`

const Menu = styled.div`
    z-index: 1000;
    background: ${p => p.theme.background1};
    position: absolute;
    height: 100%;
    width: 0;
    overflow: hidden;
    box-shadow:
  0 0px 33.3px -26px rgba(0, 0, 0, 0.073),
  0 0px 111.7px -26px rgba(0, 0, 0, 0.107),
  0 0px 500px -26px rgba(0, 0, 0, 0.18)
;
    overflow-y: auto;
    border-right: 2px solid ${p => p.theme.gray1};

    &::-webkit-scrollbar {
        width: .5rem;
    }

    &::-webkit-scrollbar-thumb {
        background: ${p => p.theme.gray1};
    }
`

const ListName = styled.span`
    font-size: 16px;
    color: ${p => p.theme.fontLight5};
    text-overflow: ellipsis;
    white-space: nowrap;
    overflow: hidden;
`

const ListLink = styled.span`
    font-size: 12px;
    transition: color 300ms ease;
    color: ${p => p.theme.fontLight4};
    text-overflow: ellipsis;
    white-space: nowrap;
    overflow: hidden;
`
