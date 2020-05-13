import styled, { css } from "styled-components";
import { Link } from "react-router-dom";

export interface IconLinkProps {
    size?: number;
}

export const IconLink = styled(Link)`
    color: ${p => p.theme.accent4};
    background: none;
    box-shadow: none;
    border-radius: 50%;
    transition: all 300ms ease;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: ${(p: IconLinkProps) => `${p.size}px`};
    padding: .5rem;

    &:hover {
        color: ${p => p.theme.accent5};
        background: ${p => p.theme.background3};
    }
`