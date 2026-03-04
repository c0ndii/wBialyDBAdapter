import { DatabaseTypes, type DatabaseEnum } from "@/api/types";
import {
  createContext,
  useMemo,
  useState,
  type PropsWithChildren,
} from "react";

type DatabaseContextValue = {
  databaseType: DatabaseEnum;
  setDatabaseType: (value: DatabaseEnum) => void;
};

// eslint-disable-next-line react-refresh/only-export-components
export const DatabaseContext = createContext<DatabaseContextValue | undefined>(
  undefined,
);

type DatabaseProviderProps = {
  defaultType?: DatabaseEnum;
};

export function DatabaseProvider({
  defaultType = DatabaseTypes.ObjectRelational,
  children,
}: PropsWithChildren<DatabaseProviderProps>) {
  const [databaseType, setDatabaseType] = useState<DatabaseEnum>(defaultType);

  const value = useMemo(
    () => ({
      databaseType,
      setDatabaseType,
    }),
    [databaseType],
  );

  return (
    <DatabaseContext.Provider value={value}>
      {children}
    </DatabaseContext.Provider>
  );
}
