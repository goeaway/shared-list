import React, { FC } from "react";
import useAuth from "../hooks/use-auth";
import styled from "styled-components";

const UserMenu : FC = () => {
    const { authData, setAuthentication } = useAuth();

    return (
        <Container>
            <UserPicture>
                <img src={authData.profile.image} />
            </UserPicture>
        </Container>
    );
}

export default UserMenu;

const Container = styled.div`
    position: absolute;
    right: 0;
    padding: .5rem;
    margin: .5rem;
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