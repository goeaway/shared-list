import React, { FC, ReactNode, useState, useEffect } from "react";
import { CSSTransition } from "react-transition-group";
import styled, { css } from "styled-components";
import useClickOutside from "../../hooks/use-click-outside";
import { AccentButton, DefaultButton } from "../style/buttons";

export interface InfoTooltipProps {
    show: boolean;
    onConfirm: () => void;
    onDismiss: () => void;
    content?: ReactNode;
    disable?: boolean;
    position?: "top" | "bottom" | "left" | "right";
}

const ConfirmTooltip: FC<InfoTooltipProps> = ({ show, disable, children, content, position, onConfirm, onDismiss }) => {
    const clickOutSideRef = useClickOutside(() => onDismiss());

    return (
        <Container ref={clickOutSideRef}>
            <CSSTransition in={show && !disable} timeout={300} classNames="show-tooltip">
                <Tooltip position={position || "top"}>
                    {content}
                    <ControlBar>
                        <AccentButton type="button" onClick={onConfirm}>Confirm</AccentButton>
                        <DefaultButton type="button" onClick={onDismiss}>Cancel</DefaultButton>
                    </ControlBar>
                </Tooltip>
            </CSSTransition>
            <div>
                {children}
            </div>
        </Container>
    );
}

export default ConfirmTooltip;

const Container = styled.div`
    position: relative;
    display: flex;
    align-items: center;

    .show-tooltip-enter {
        opacity: 0;
        visibility: hidden;
    }

    .show-tooltip-enter-active {
        opacity: 1;
        visibility: visible;
        transition: all 300ms;
    }

    .show-tooltip-enter-done {
        opacity: 1;
        visibility: visible;
    }

    .show-tooltip-exit {
        opacity: 1;
        visibility: visible;
    }

    .show-tooltip-exit-active {
        opacity: 0;
        visibility: hidden;
        transition: all 300ms;
    }

    .show-tooltip-exit-done {
        opacity: 0;
        visibility: hidden;
    }
`

interface TooltipProps {
    position: "top" | "bottom" | "left" | "right";
}

const Tooltip = styled.div`
    z-index: 100000;
    background: ${p => p.theme.background1};
    position: absolute;
    padding: 1rem;
    font-size: 12px;
    line-height: 16px;
    border-radius: 3px;
    opacity: 0;
    visibility: hidden;
    box-shadow: 0px 2px 15px -1px rgba(0,0,0,.35);

    ${(p: TooltipProps) => p.position === "top" && css`
        bottom: 70%;
        left: 50%;
        transform: translateX(-50%);
    `}

    ${(p: TooltipProps) => p.position === "bottom" && css`
        top: 70%;
        left: 50%;
        transform: translateX(-50%);
    `}

    ${(p: TooltipProps) => p.position === "left" && css`
        right: 150%;
        top: 50%;
        transform: translateY(-50%);
    `}
    
    ${(p: TooltipProps) => p.position === "right" && css`
        left: 150%;
        top: 50%;
        transform: translateY(-50%);
    `}
`

const ControlBar = styled.div`
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: .5rem; 

    > *:first-child {
        margin-right: .2rem;
    }

    > *:last-child {
        margin-left: .2rem;
    }
`
