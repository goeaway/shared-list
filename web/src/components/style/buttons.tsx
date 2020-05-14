import styled, {css} from "styled-components";

export interface ButtonProps {
    prepend?: boolean;
    append?: boolean;
    hideTextXS?: boolean;
}

export const Button = styled.button`
    border-radius: ${(p: ButtonProps) => p.prepend ? "3px 0 0 3px" : p.append ? "0 3px 3px 0" : "3px"};
    border: none;
    padding: .5rem;
    cursor: pointer;
    display:flex;
    align-items:center;
    box-shadow:
        0 3.6px 9.6px rgba(0, 0, 0, 0.028),
        0 12.1px 32.2px rgba(0, 0, 0, 0.042),
        0 54px 144px rgba(0, 0, 0, 0.07);

    position: relative;
    transition: all .2s ease;
    outline: none;

    ${(p: ButtonProps) => p.hideTextXS && css`
        @media(max-width:${p => p.theme.xs}px) {
            span {
                display:none;
            }
        }
    `}

    &:disabled {
        cursor: default;
    }
`

export const AccentButton = styled(Button)`
    background: ${p => p.theme.accent4}; 
    color: ${p => p.theme.fontLight1};

    &:hover {
        background: ${p => p.theme.accent5}; 
    }

    &:disabled {
        background: ${p => p.theme.accent1};
    }
`

export const DefaultButton = styled(Button)`
    background: ${p => p.theme.default1}; 
    color: ${p => p.theme.fontDark1};

    &:hover {
        background: ${p => p.theme.default2}; 
    }

    &:disabled {
        background: ${p => p.theme.default1};
    }
`

export const SuccessButton = styled(Button)`
    background: ${p => p.theme.success4}; 
    color: ${p => p.theme.fontLight1};

    &:hover {
        background: ${p => p.theme.success5}; 
    }

    
    &:disabled {
        background: ${p => p.theme.success4};
    }
`

export const InvertedAccentButton = styled(Button)`
    color: ${p => p.theme.accent4};
    background: transparent;
    transition: color 300ms ease;
    box-shadow: none;

    &:hover {
        color: ${p => p.theme.accent5}; 
    }

    &:disabled {
        color: ${p => p.theme.accent1};
    }
`

export interface IconButtonProps {
    square?: boolean;
}

export const IconButton = styled.button`
    color: ${p => p.theme.accent4};
    background: none;
    box-shadow: none;
    transition: all 300ms ease;
    display: flex;
    justify-content: center;
    align-items: center;
    padding: .5rem;
    border: none;
    outline: none;
    cursor: pointer;

    &:hover {
        color: ${p => p.theme.accent5};
        background: ${p => p.theme.background3};
    }

    ${(p: IconButtonProps) => !p.square && css`
        border-radius: 50%;
    `}
`