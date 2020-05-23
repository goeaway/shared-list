import React, { FC, ReactNode, useState, useEffect, useLayoutEffect, useRef } from "react";
import { CSSTransition } from "react-transition-group";
import styled, { css } from "styled-components";

export interface InfoTooltipProps {
    content?: ReactNode;
    disable?: boolean;
    position?: "top" | "bottom" | "left" | "right";
    delay?: number;
}

const InfoTooltip: FC<InfoTooltipProps> = ({ delay, disable, children, content, position }) => {
    const [show, setShow] = useState(false);
    const [hovering, setHovering] = useState(false);

    useEffect(() => {
        if(hovering) {
            const timeout = setTimeout(() => {
                if(hovering) {
                    setShow(true);
                }
            }, !delay || delay < 0 ? 500 : delay);

            return () => clearTimeout(timeout);
        } else if(show) {
            setShow(false);
        }
    }, [hovering]);


    const onMouseEnterHandler = () => {
        setHovering(true);
    }

    const onMouseOutHandler = () => {
        setHovering(false);
    }

    return (
        <Container>
            <CSSTransition in={show && !disable} timeout={300} classNames="show-tooltip">
                <Tooltip position={position || "top"}>
                    {content}
                </Tooltip>
            </CSSTransition>
            <span onMouseEnter={onMouseEnterHandler} onMouseLeave={onMouseOutHandler}>
                {children}
            </span>
        </Container>
    );
}

export default InfoTooltip;

const Container = styled.span`
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
    position: absolute;
    color: ${p => p.theme.fontLight1};
    background: ${p => p.theme.accent4};
    padding: .2rem .5rem;
    font-size: 12px;
    line-height: 18px;
    border-radius: 3px;
    opacity: 0;
    visibility: hidden;
    box-shadow: 0px 2px 15px -1px rgba(0,0,0,.35);
    white-space: nowrap;

    ${(p: TooltipProps) => p.position === "top" && css`
        bottom: 150%;
        left: 50%;
        transform: translateX(-50%);
    `}

    ${(p: TooltipProps) => p.position === "bottom" && css`
        top: 150%;
        left: 50%;
        transform: translateX(-50%);
    `}

    ${(p: TooltipProps) => p.position === "left" && css`
        left: -200%;
        top: 50%;
        transform: translateY(-50%);
    `}
    
    ${(p: TooltipProps) => p.position === "right" && css`
        right: -200%;
        top: 50%;
        transform: translateY(-50%);
    `}
`
