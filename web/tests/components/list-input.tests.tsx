import { render, screen, fireEvent } from "@testing-library/react";
import ListInput from "@src/components/list-input";
import { SinonStub, createSandbox } from "sinon";
import React from "react";

describe("Tests the list input component", () => {
    let onAdd: SinonStub;
    let onLoseFocus: SinonStub;
    let onRequestFocus: SinonStub;
    const sandbox = createSandbox();

    beforeEach(() => {
        sandbox.restore();
        onAdd = sandbox.stub();
        onLoseFocus = sandbox.stub();
        onRequestFocus = sandbox.stub();
    });

    it("should render with an input", () => {
        render(<ListInput onAdd={onAdd} clickOutsideRef={null} onLoseFocus={onLoseFocus} onRequestFocus={onRequestFocus} />);
        expect(screen.getByPlaceholderText("Add an item...")).not.toBeUndefined();
    });

    it("should render with an add button", () => {
        render(<ListInput onAdd={onAdd} clickOutsideRef={null} onLoseFocus={onLoseFocus} onRequestFocus={onRequestFocus} />);
        expect(screen.getByRole("add-item-button")).not.toBeUndefined();
    });

    it("should call the onAdd prop when the add button is pressed and a value is in the input", () => {
        const EXPECTED_VALUE = "something";
        render(<ListInput onAdd={onAdd} clickOutsideRef={null} onLoseFocus={onLoseFocus} onRequestFocus={onRequestFocus} />);
        fireEvent.change(screen.getByPlaceholderText("Add an item..."), { target: { value: EXPECTED_VALUE }})
        fireEvent.click(screen.getByRole("add-item-button"));
        expect(onAdd.calledOnce).toBe(true);
        expect(onAdd.firstCall.args[0]).toBe(EXPECTED_VALUE);
    });

    it("should not call the onAdd prop when the add button is pressed but the value is empty", () => {
        render(<ListInput onAdd={onAdd} clickOutsideRef={null} onLoseFocus={onLoseFocus} onRequestFocus={onRequestFocus} />);
        fireEvent.click(screen.getByRole("add-item-button"));
        expect(onAdd.calledOnce).toBe(false);
    });

    it("should focus the input when the focus prop is true", () => {
        render(<ListInput onAdd={onAdd} clickOutsideRef={null} onLoseFocus={onLoseFocus} focus onRequestFocus={onRequestFocus} />);
        expect(document.activeElement).toBe(screen.getByPlaceholderText("Add an item..."));
    });

    it("should not focus the input when the focus prop is false", () => {
        render(<ListInput onAdd={onAdd} clickOutsideRef={null} onLoseFocus={onLoseFocus} onRequestFocus={onRequestFocus} />);
        expect(document.activeElement).not.toBe(screen.getByPlaceholderText("Add an item..."));
    });
});