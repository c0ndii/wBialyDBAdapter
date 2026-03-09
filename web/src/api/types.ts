export const DatabaseTypes = {
  ObjectRelational: 0,
  Relational: 1,
  NoSQL: 2,
} as const;

export type DatabaseEnum = (typeof DatabaseTypes)[keyof typeof DatabaseTypes];

export type BaseRequest = {
  databaseType?: DatabaseEnum;
};

export type EndpointRequest = BaseRequest & {
  pageIndex: number;
  pageSize: number;
};

export type PostsRequest<T> = BaseRequest & {
  data: T;
};

export type EndpointResponse<T> = {
  data: T;
  totalCount: number;
};

export interface Tag {
  id: string;
  name: string;
}

export interface Post {
  id: string;
  title: string;
  description: string;
  author: string;
  addDate: Date;
  link: string;
  place: string;
}

export interface User {
  id: number;
  username: string;
}

export type LoginUser = {
  login: string;
  password: string;
};

export type UserSecuritySettings = {
  isLockoutEnabled: boolean;
  maxFailedLoginAttempts: number;
  isPasswordManagerEnabled: boolean;
};

export type UserSecurityStats = {
  lastFailedLoginAtUtc: string | null;
  lastSuccessfulLoginAtUtc: string | null;
  failedLoginCountSinceLastSuccess: number;
  failedLoginCountTotal: number;
  successfulLoginCount: number;
  isLocked: boolean;
  lockedUntilUtc: string | null;
  nextAllowedLoginAtUtc: string | null;
};

export type UserSecurityOverview = {
  settings: UserSecuritySettings;
  stats: UserSecurityStats;
};

export type LoginAuditLog = {
  id: number;
  userId: number | null;
  loginIdentifier: string;
  attemptedAtUtc: string;
  isSuccessful: boolean;
  isExistingUser: boolean;
  appliedDelaySeconds: number;
  lockedUntilUtc: string | null;
  failureCategory: string | null;
  ipAddress: string | null;
};

export const LogsScope = {
  Mine: "mine",
  All: "all",
} as const;

export type LogsScopeEnum = (typeof LogsScope)[keyof typeof LogsScope];

export type RegisterUser = Pick<User, "username"> & LoginUser;

export type MessageUser = {
  id: number;
  username: string;
};

export type Message = {
  id: number;
  content: string;
  createdAt: string;
  modifiedAt: string;
  latestModifyUsername: string;
  userId: number;
  user: MessageUser;
  canModify: MessageUser[];
};

export type MessageListResponse = {
  messages: Message[];
};

export type CreateMessageInput = {
  content: string;
};

export type UpdateMessageInput = {
  messageId: number;
  content: string;
};

export type UpdateMessageEditorsInput = {
  userIds: number[];
  messageId: number;
};
