export function Header() {
    return (
        <header className="relative z-50 flex h-16 items-center justify-between border-b bg-white px-8">

            <div>
                <h1 className="text-xl font-semibold">
                    Urban Forest Asset Management System
                </h1>

                <p className="text-sm text-muted-foreground">
                    City of Vancouver Demo
                </p>
            </div>

            <div className="rounded-full bg-green-100 px-4 py-2 text-sm font-medium text-green-700">
                Arborist
            </div>

        </header>
    );
}