import { Alias } from "./alias.model";

export interface Group {
    groupId: string;
    titleSection: string;
    isRepeatCard: boolean;
    gridDisplay: number;
    disabled: boolean;
    aliases?: Alias[];
    IdTemplate:string;
  }