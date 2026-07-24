export function MapLegend() {
    return (
        <div className="absolute right-4 top-4 z-10 rounded-lg border bg-white p-4 shadow-lg">
            <h3 className="mb-3 font-semibold">
                Tree Health
            </h3>

            <div className="space-y-2 text-sm">
                <div>🔵 Excellent</div>
                <div>🟢 Good</div>
                <div>🟡 Fair</div>
                <div>🟠 Poor</div>
                <div>🔴 Dead</div>
            </div>
        </div>
    );
}