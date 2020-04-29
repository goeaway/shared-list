export interface ListDTO {
    id: string;
    items: Array<ListItemDTO>,
    name: string;
}

export interface ListItemDTO {
    value: string;
    notes: string;
    id: string;
    complete: boolean;
}

