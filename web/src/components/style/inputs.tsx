import styled, {css} from "styled-components";

export interface InputProps {
    withAddon?: boolean;
}

export const Input = styled.input`
    flex: 1 1 auto;
    border: 2px solid ${p => p.theme.gray1};
    border-radius: ${(p: InputProps) => p.withAddon ? "3px 0 0 3px" : "3px"};
    padding: .5rem;
    background: ${p => p.theme.background2};
    outline: none;
    color: ${p => p.theme.fontDark2};
    cursor: text;
`

export const SubtleInput = styled.input`
    font-size: inherit;
    color: inherit;
    flex: 1 1 auto;
    border: 2px solid ${p => p.theme.gray1};
    border-radius: 3px;
    padding: .25rem;
    background: ${p => p.theme.background2};
    outline: none;
    cursor: text;
`