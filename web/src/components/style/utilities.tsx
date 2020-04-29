import styled, { css } from "styled-components";

export interface SpacerProps {
    marginX?: number;
    marginY?: number;
}

export const Spacer = styled.div`
    ${(p: SpacerProps) => p.marginX && css`
        margin-left: ${(p: SpacerProps) => p.marginX}px;
        margin-right: ${(p: SpacerProps) => p.marginX}px;
    `}

    ${(p: SpacerProps) => p.marginY && css`
        margin-top: ${(p: SpacerProps) => p.marginY}px;
        margin-bottom: ${(p: SpacerProps) => p.marginY}px;
    `}
`