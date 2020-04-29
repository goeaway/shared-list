import { ListItemDTO } from "../../src/types";

export default function createListItem(id?: string) : ListItemDTO {
    return {
        id: id || "id",
        value: "value",
        notes: "notes",
        complete: false
    }
}