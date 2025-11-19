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
  ClearAttendances = 5,
  ClearEmployees = 6,
  ClearData = 7,
  RestartDevice = 8,
  SyncAttendances = 9,
  SyncUsers = 10,
}