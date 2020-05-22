import * as React from "react";
import { FC } from "react";
import styled from "styled-components";
import { FaSearchMinus } from "react-icons/fa";
import { Link } from "react-router-dom";

const NotFoundPage : FC = () => {
    return (
        <Container>
            <FaSearchMinus size={200}/>
            <Text>Page Not Found. <Link to="/" >Go Home</Link></Text>
        </Container>    
    );
};

export default NotFoundPage;

const Container = styled.div`
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100vw;
    height: 100vh;
    flex-direction: column;
    color: ${p => p.theme.background3};

    > *:first-child {
        margin-bottom: 1rem;
    }
`

const Text = styled.span`
    font-size: 14px;
    color: ${p => p.theme.fontLight4};

    a {
        color: ${p => p.theme.fontDark1};

        &:visited {
            color: ${p => p.theme.fontDark2};
        }

        &:hover {
            color: ${p => p.theme.fontDark3};
        }
    }

`