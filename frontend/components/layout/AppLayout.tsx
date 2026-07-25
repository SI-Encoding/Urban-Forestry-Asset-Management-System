import { ReactNode } from "react";

import { Sidebar } from "./Sidebar";
import { Header } from "./Header";

interface AppLayoutProps {
    children: ReactNode;
}

export function AppLayout({
    children,
}: AppLayoutProps) {

    return (

        <div className="flex h-screen">

            <Sidebar />

            <div className="flex flex-1 flex-col">

                <Header />

                <main className="flex-1 overflow-hidden p-8 bg-gray-50">

                    {children}

                </main>

            </div>

        </div>

    );
}