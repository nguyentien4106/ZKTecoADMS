export interface DeviceCommandRequest {
  commandType: DeviceCommandTypes;
  priority?: number;
}

export enum DeviceCommandTypes {
  AddUser,
  DeleteUser,
  UpdateUser,
  InitialAttendances,
  InitialUsers,
  CLEAR_ATTENDANCEs = 5,
  CLEAR_USERS = 6,
  CLEAR_DATA = 7,
  RESTART_DEVICE = 8,
  SYNC_ATTENDANCES = 9,
  SYNC_USERS = 10,
}