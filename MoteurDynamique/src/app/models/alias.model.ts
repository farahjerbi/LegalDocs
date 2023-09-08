import { typeSettings } from "./typeSetting.model";

export interface Alias {
    id: string;
    entityName: string;
    title: string;
    type: string;
    display: string;
    source: {
      sourceType: string;
    };
    typeSetting:typeSettings
    groupId: string;
  }