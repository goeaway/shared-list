import React, { FC, useState } from "react";
import useAuth from "../hooks/use-auth";
import styled, { css } from "styled-components";
import useClickOutside from "../hooks/use-click-outside";
import { FaPowerOff } from "react-icons/fa";

const UserMenu : FC = () => {
    const { authData, setAuthentication } = useAuth();
    const [menuOpen, setMenuOpen] = useState(false);
    const clickOutsiteRef = useClickOutside(() => menuOpen && setMenuOpen(false));

    const userPictureClickHandler = () => {
        setMenuOpen(o => !o);
    }

    const logoutClickHandler = () => {
        setAuthentication(undefined);

        setMenuOpen(false);
    }

    return (
        <Container ref={clickOutsiteRef}>
            <UserPicture onClick={userPictureClickHandler}>
                <img src={authData.profile.image} />
            </UserPicture>
            <Menu show={menuOpen}>
                <MenuButton onClick={logoutClickHandler}>
                    <FaPowerOff />&nbsp;Sign out
                </MenuButton>
            </Menu>
        </Container>
    );
}

export default UserMenu;

const Container = styled.div`
    position: relative;
`

const UserPicture = styled.button`
    outline: none;
    background: none;
    border: none;
    width: 35px;
    height: 35px;
    border-radius: 50%;
    overflow:hidden;
    padding: 0;
    cursor: pointer;
    
    img {
        width: 35px;
        height: auto;
    }
`

interface MenuProps {
    show?: boolean;
}

const Menu = styled.div`
    position: absolute;
    bottom: -40px;
    right: 0;
    opacity: 0;
    visibility: hidden;
    transition: all 200ms;
    overflow: hidden;
    border-radius: 3px;
    min-width: 100px;
    
    border: 1px solid ${p => p.theme.gray1};

    ${(p: MenuProps) => p.show && css`
        opacity: 1;
        visibility: visible;
    `}

    box-shadow:
  0 0.8px 2.2px rgba(0, 0, 0, 0.02),
  0 2px 5.3px rgba(0, 0, 0, 0.028),
  0 3.8px 10px rgba(0, 0, 0, 0.035),
  0 6.7px 17.9px rgba(0, 0, 0, 0.042),
  0 12.5px 33.4px rgba(0, 0, 0, 0.05),
  0 30px 80px rgba(0, 0, 0, 0.07)
;
`

const MenuButton = styled.button`
    padding: .5rem;
    background: transparent;
    transition: all 300ms;
    border: none; 
    cursor: pointer;
    width: 100%;
    color: ${p => p.theme.fontLight5};
    display: flex;
    align-items: center;
    justify-content: space-around;

    &:hover {
        background: ${p => p.theme.background2};
        color: ${p => p.theme.fontDark2};
    }
`

const MenuItem = styled.div`
    padding: .5rem;
    transition: background 300ms ease;
`
