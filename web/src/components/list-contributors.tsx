import React, { FC } from "react";
import styled from "styled-components";
import InfoTooltip from "./tooltips/info-tooltip";

export interface ListContributorsProps {
    contributors: Array<string>;
}

const ListContributors : FC<ListContributorsProps> = ({contributors}) => {
    return (
        <Container>
            {contributors.length === 0 && "No other contributors"}
            {contributors.length > 0 && contributors.length < 4 && `With ${contributors.join(", ")}` }
            {contributors.length >= 4 && 
                <Info>
                    <InfoTooltip delay={100} position="bottom" content={<span>{contributors.map(c => <span>{c}<br /></span>)}</span>}>
                        With {contributors.slice(0, 3).join(", ")} <More>+{contributors.length - 3} more</More>
                    </InfoTooltip>
                </Info>
            }
        </Container>
    )
};

export default ListContributors;

const Container = styled.span`
    margin-top: .25rem;
    font-size: 12px;
    color: ${p => p.theme.fontLight4};
`
    
const Info = styled.span`
    display: inline-flex;
    cursor: pointer;
    transition: color 300ms;

    &:hover {
        color: ${p => p.theme.fontDark2};
    }
`

const More = styled.span`
    white-space: nowrap;
`
