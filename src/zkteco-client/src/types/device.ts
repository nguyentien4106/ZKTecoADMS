export interface DeviceCommandRequest {
  commandType: DeviceCommandTypes;
  priority?: number;
}

export enum DeviceCommandTypes {
  CLEAR_ATTENDANCEs = 5,
  CLEAR_USERS = 6,
  CLEAR_DATA = 7,
  RESTART_DEVICE = 8,
  SYNC_ATTENDANCES = 9
}