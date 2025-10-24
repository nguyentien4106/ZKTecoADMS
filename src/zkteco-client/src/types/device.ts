export interface DeviceCommandRequest {
  commandType: DeviceCommandTypes;
}

export enum DeviceCommandTypes {
    CLEAR_ATTENDANCEs = 0,
    CLEAR_USERS = 1,
    CLEAR_DATA = 2,
    RESTART_DEVICE = 3,
}