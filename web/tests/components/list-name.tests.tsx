import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import ListName from "@src/components/list-name";
import { SinonStub, createSandbox } from "sinon";

describe("Tests the list name component", () => {
    let onChange: SinonStub;
    let onEditChanged: SinonStub;
    const sandbox = createSandbox();

    beforeEach(() => {
        sandbox.restore();
        onChange = sandbox.stub();
        onEditChanged = sandbox.stub();
    });

    it("should render with the list name prop in a span", () => {
        const EXPECTED_NAME = "name";
        render(<ListName name={EXPECTED_NAME} onChange={onChange} onEditChanged={onEditChanged} />);
        expect(screen.getByRole("list-name-span")).not.toBeUndefined();
        expect(screen.getByRole("list-name-span").textContent).toBe(EXPECTED_NAME);
    });

    it("should not render with input", () => {
        render(<ListName name="name" onChange={onChange} onEditChanged={onEditChanged} />);
        expect(screen.queryByRole("list-name-input")).toBeNull();
    });

    it("should hide span on span click", () => {
        render(<ListName name="name" onChange={onChange} onEditChanged={onEditChanged} />);
        fireEvent.click(screen.getByRole("list-name-span"));
        expect(screen.queryByRole("list-name-span")).toBeNull();
    });

    it("should show an input on span click", () => {
        render(<ListName name="name" onChange={onChange} onEditChanged={onEditChanged} />);
        fireEvent.click(screen.getByRole("list-name-span"));
        expect(screen.getByRole("list-name-input")).not.toBeUndefined();
    });

    it("should focus input on span click", () => {
        render(<ListName name="name" onChange={onChange} onEditChanged={onEditChanged} />);
        fireEvent.click(screen.getByRole("list-name-span"));
        expect(document.activeElement).toBe(screen.getByRole("list-name-input"));
    });

    it("should call the onEditChanged callback on span click", () => {
        render(<ListName name="name" onChange={onChange} onEditChanged={onEditChanged} />);
        fireEvent.click(screen.getByRole("list-name-span"));
        expect(onEditChanged.calledTwice).toBe(true);
    });
});