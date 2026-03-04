import {
  createContext,
  useMemo,
  useState,
  type PropsWithChildren,
} from "react";

type ThemeEnum = "light" | "dark";

type ThemeContextValue = {
  theme: ThemeEnum;
  setTheme: (value: ThemeEnum) => void;
};

// eslint-disable-next-line react-refresh/only-export-components
export const ThemeContext = createContext<ThemeContextValue | undefined>(
  undefined,
);

type ThemeProviderProps = {
  defaultTheme?: ThemeEnum;
};

export function ThemeProvider({
  defaultTheme = "light",
  children,
}: PropsWithChildren<ThemeProviderProps>) {
  const [theme, setTheme] = useState<ThemeEnum>(defaultTheme);

  const value = useMemo(
    () => ({
      theme,
      setTheme,
    }),
    [theme],
  );

  return (
    <ThemeContext.Provider value={value}>{children}</ThemeContext.Provider>
  );
}
