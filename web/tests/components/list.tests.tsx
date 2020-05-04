import List from "../../src/components/list";
import React from "react";
import { ListDTO } from "../../src/types";
import createList from "../utils/create-list";
import createListItem from "../utils/create-list-item";
import { render, screen, fireEvent } from "@testing-library/react";
import "jest-styled-components"; // somehow this allows a test to pass, don't remove
import { createSandbox } from "sinon";
import { ToastProvider } from "react-toast-notifications";

describe("Tests the list component", () => {
    let dto: ListDTO;
    const sandbox = createSandbox();

    const renderList = (dto: ListDTO, canCopy?: boolean) => {
        render(
            <ToastProvider>
                <List list={dto} canCopy={canCopy} />
            </ToastProvider>
        );
    }

    beforeEach(() => {
        dto = createList();
        sandbox.restore();
    });

    it("should render with a list input component", () => {
        renderList(dto);
        expect(screen.getByPlaceholderText("Add an item...")).not.toBeUndefined();
    });

    it("should render with a copy list button", () => {
        renderList(dto);
        expect(screen.getByText("Copy List URL")).not.toBeUndefined();
    });

    it("should render with an uncomplete all button", () => {
        renderList(dto);
        expect(screen.getByText("Uncomplete All")).not.toBeUndefined();
    });

    it("should render with a toggle hide complete button", () => {
        renderList(dto);
        expect(screen.getByText("Hide Completed")).not.toBeUndefined();
    });

    it("should render with a list name component", () => {
        renderList(dto);
        expect(screen.getByRole("list-name")).not.toBeUndefined();
    });

    it("should render with an empty list component if no list items exist on the ListDTO prop", () => {
        dto.items = [];
        renderList(dto);
        expect(screen.getByRole("list-empty")).not.toBeUndefined();
    });

    it("should render with a list item component for each list item on the ListDTO prop", () => {
        const complete = createListItem("0");
        complete.completed = true;
        dto.items = [complete, createListItem("1")];
        renderList(dto);
        expect(screen.getAllByRole("list-item").length).toBe(2);
    });

    it("should focus the list input component when spacebar is pressed", () => {
        renderList(dto);
        fireEvent.keyPress(screen.getByRole("list"), {
            key: " ",
            code: "Space",
        });

        expect(document.activeElement).toBe(screen.getByPlaceholderText("Add an item..."));
    });

    it("should not focus the list input component when spacebar is pressed if list name is being edited", () => {
        renderList(dto);
        fireEvent.doubleClick(screen.getByRole("list-name-span"));

        fireEvent.keyPress(screen.getByRole("list"), {
            key: " ",
            code: "Space",
        });

        expect(document.activeElement).not.toBe(screen.getByPlaceholderText("Add an item..."));
    });

    it("should not focus the list input component when spacebar is pressed if a list item is being edited", () => {
        renderList(dto);
        fireEvent.doubleClick(screen.getByRole("list-item-value"));

        fireEvent.keyPress(screen.getByRole("list"), {
            key: " ",
            code: "Space",
        });

        expect(document.activeElement).not.toBe(screen.getByPlaceholderText("Add an item..."));
    });
    
    it("should add an item to the list", () => {
        // act by typing into the list input and clicking the button, we should then see another item in the list
        dto.items = [];
        renderList(dto);
        fireEvent.change(screen.getByPlaceholderText("Add an item..."), { target: { value: "new item" }});
        fireEvent.click(screen.getByRole("add-item-button"));

        expect(screen.getAllByRole("list-item").length).toBe(1);
    });

    it("should delete an item from the list", () => {
        renderList(dto);
        // open the context menu
        fireEvent.click(screen.getByRole("list-item-value"), { button: 2 });
        // click the delete option
        fireEvent.click(screen.getByText("Delete"));

        expect(screen.queryAllByRole("list-item").length).toBe(0);
    });

    it("should change hide completed text to show completed when hide complete button is clicked", () => {
        renderList(dto);
        fireEvent.click(screen.getByText("Hide Completed"));
        expect(screen.queryByText("Hide Completed")).toBeNull();
        expect(screen.getByText("Show Completed")).not.toBeUndefined();
    });

    it("should hide completed items when hide complete button is clicked", () => {
        const completed = createListItem("complete");
        completed.completed = true;
        dto.items = [createListItem("2"), completed, createListItem("1")];

        renderList(dto);
        fireEvent.click(screen.getByText("Hide Completed"));
        expect(screen.getAllByRole("list-item").length).toBe(2);
    });

    it("should remove item from display when hide complete is in effect and an item is completed", () => {
        dto.items = [createListItem("1"), createListItem("2")];
        renderList(dto);
        fireEvent.click(screen.getByText("Hide Completed"));
        fireEvent.click(screen.getAllByRole("list-item-check")[0]);

        expect(screen.queryAllByRole("list-item").length).toBe(1);
    });

    it("should copy the current location href to the clipboard on copy url button click", async () => {
        renderList(dto);
        const writeStub = sandbox.stub(navigator.clipboard, "writeText");
        fireEvent.click(screen.getByText("Copy List URL"));
        expect(writeStub.calledOnce).toBe(true);
        expect(writeStub.firstCall.args[0]).toBe("http://localhost")
    });

    it("should show a toast when the copy url button is clicked", () => {
        fail();
    });

    it("should set all completed items as incomplete", () => {
        const complete = createListItem("0");
        complete.completed = true;
        dto.items = [complete];
        renderList(dto);
        fireEvent.click(screen.getByText("Uncomplete All"));
        expect(screen.getByRole("list-item-check-incomplete")).not.toBeUndefined();
        expect(screen.queryByRole("list-item-check-complete")).toBeNull();
    });
});