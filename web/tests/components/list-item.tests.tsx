import { render, screen, fireEvent, waitFor, waitForElementToBeRemoved } from "@testing-library/react";
import React from "react";
import ListItem from "../../src/components/list-item";
import { ListItemDTO } from "../../src/types";
import createListItem from "../utils/create-list-item";
import { SinonStub, createSandbox } from "sinon";
import { DragDropContext, Droppable } from "react-beautiful-dnd";
import "jest-styled-components";

describe("Tests the list item component", () => {
    const sandbox = createSandbox();

    let dto: ListItemDTO;
    let onItemEdited: SinonStub;
    let onEditingChanged: SinonStub;
    let onDelete: SinonStub;
    let onDragEnd: SinonStub;

    beforeEach(() => {
        dto = createListItem();

        sandbox.restore();
        onItemEdited = sandbox.stub();
        onEditingChanged = sandbox.stub();
        onDelete = sandbox.stub();
        onDragEnd = sandbox.stub();
    });

    const renderDraggableListItem = (listItem: ListItemDTO, index: number = 0, hide?: boolean) => {
        render(
            <DragDropContext onDragEnd={onDragEnd}>
                <Droppable droppableId="test-drop">
                    {provided => (
                        <div {...provided.droppableProps} ref={provided.innerRef}>
                            <ListItem 
                                listItem={listItem} 
                                index={index} 
                                hide={hide}
                                onItemEdited={onItemEdited}
                                onEditingChanged={onEditingChanged} 
                                onDelete={onDelete} />
                        </div>
                    )}
                </Droppable>
            </DragDropContext>);
    }

    it("should render with a span containing the ListItemDTO.value value", () => {
        renderDraggableListItem(dto);
        expect(screen.getByRole("list-item-value")).not.toBeUndefined();
    });

    it("should render with no input", () => {
        renderDraggableListItem(dto);
        expect(screen.queryByRole("list-item-input")).toBeNull();
    });

    it("should render with a checkbox", () => {
        renderDraggableListItem(dto);
        expect(screen.getByRole("list-item-check")).not.toBeUndefined();
    });

    it("should call the onCompletionChanged callback when the checkbox input is checked", () => {
        const testDTO = { id: "something", notes: "testnotes", value: "testvalue", completed: false };
        renderDraggableListItem(testDTO);
        fireEvent.click(screen.getByRole("list-item-check"));
        expect(onItemEdited.calledOnce).toBe(true);
        expect(onItemEdited.firstCall.args[0]).toBe(testDTO.id);
        expect(onItemEdited.firstCall.args[1]).toStrictEqual({ completed: true, notes: "testnotes", value: "testvalue", id: "something"});
    });

    it("should show a context menu when the list item container is right clicked", () => {
        renderDraggableListItem(dto);
        fireEvent.click(screen.getByRole("list-item-value"), { button: 2 });
        expect(screen.getByRole("menu")).not.toBeUndefined();
    });

    it("should hide span after a double click on span", () => {
        renderDraggableListItem(dto);
        fireEvent.click(screen.getByRole("list-item-value"));

        expect(screen.queryByRole("list-item-value")).toBeNull();
    });

    it("should show input after a double click on span", () => {
        renderDraggableListItem(dto);
        fireEvent.click(screen.getByRole("list-item-value"));

        expect(screen.getByRole("list-item-input")).not.toBeUndefined();
    });

    it("should have item value in input after a double click on span", () => {
        const EXPECTED_VALUE = "test value";
        dto.value = EXPECTED_VALUE;
        renderDraggableListItem(dto);
        fireEvent.click(screen.getByRole("list-item-value"));

        const input = screen.getByRole("list-item-input") as HTMLInputElement;
        expect(input.value).toBe(EXPECTED_VALUE);
    });

    it("should focus the input when editing", () => {
        renderDraggableListItem(dto);
        fireEvent.click(screen.getByRole("list-item-value"));
        expect(document.activeElement).toBe(screen.getByRole("list-item-input"));
    });
    
    it("should not hide checkbox when editing", () => {
        renderDraggableListItem(dto);
        fireEvent.click(screen.getByRole("list-item-value"));
        expect(screen.getByRole("list-item-check")).not.toBeUndefined();
    });

    it("should call the onEditingChanged callback when starting to edit", () => {
        renderDraggableListItem(dto);
        fireEvent.click(screen.getByRole("list-item-value"));
        expect(onEditingChanged.calledTwice).toBe(true);
        expect(onEditingChanged.secondCall.args[0]).toBe(true);
    });

    it("should display none if hide prop is true and the list item is complete", () => {
        dto.completed = true;
        renderDraggableListItem(dto, 0, true);
        expect(screen.queryByRole("list-item")).toBeNull();
    });

    it("should display if hide prop is true but the list item is not complete", () => {
        renderDraggableListItem(dto, 0, true);
        expect(screen.getByRole("list-item")).not.toHaveStyleRule("display", "none");
    });

    it("should display if hide prop is false but list item is complete", () => {
        renderDraggableListItem(dto, 0, true);
        expect(screen.getByRole("list-item")).not.toHaveStyleRule("display", "none");
    });
});