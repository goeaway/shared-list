import React, { FC } from "react";
import useAuth from "../hooks/use-auth";
import styled from "styled-components";

const UserMenu : FC = () => {
    const { authData, setAuthentication } = useAuth();

    return (
        <Container>User Menu</Container>
    );
}

export default UserMenu;

const Container = styled.div`
    position: absolute;
    right: 0;
`