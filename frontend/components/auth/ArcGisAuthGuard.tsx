"use client";

import {
    useEffect,
    useState,
} from "react";


import {
    getArcGisAuthStatus,
    getArcGisLoginUrl,
    logoutArcGis,
} from "@/services/arcgis-auth-service";



interface ArcGisAuthGuardProps {

    children: React.ReactNode;

}



export function ArcGisAuthGuard({
    children,
}: ArcGisAuthGuardProps) {


    const [checking, setChecking] =
        useState(true);


    const [authenticated, setAuthenticated] =
        useState(false);


    async function disconnect() {

        await logoutArcGis();

        setAuthenticated(false);

    }
    useEffect(() => {

        async function checkAuth() {

            try {

                const status =
                    await getArcGisAuthStatus();


                setAuthenticated(
                    status.authenticated
                );

            }
            catch (error) {

                console.error(
                    "Unable to check ArcGIS authentication",
                    error
                );


                setAuthenticated(false);

            }
            finally {

                setChecking(false);

            }

        }


        checkAuth();


    }, []);





    if (checking) {

        return (

            <div className="flex h-64 items-center justify-center">

                <p className="text-muted-foreground">
                    Checking ArcGIS connection...
                </p>

            </div>

        );

    }





    if (!authenticated) {

        return (

            <div className="flex h-64 flex-col items-center justify-center space-y-4 rounded-lg border bg-white p-8">


                <h2 className="text-xl font-semibold">
                    ArcGIS Connection Required
                </h2>


                <p className="max-w-md text-center text-muted-foreground">

                    UFAMS requires an ArcGIS connection
                    before synchronizing spatial data.

                </p>



                <button

                    className="
                        rounded-md
                        bg-primary
                        px-5
                        py-2
                        text-primary-foreground
                        hover:opacity-90
                    "

                    onClick={() => {

                        window.location.href =
                            getArcGisLoginUrl();

                    }}

                >

                    Connect ArcGIS

                </button>


            </div>

        );

    }





    return (

        <div className="space-y-4">

            <div className="flex justify-end">

                <button
                    className="
                        rounded-md
                        border
                        px-4
                        py-2
                        text-sm
                        hover:bg-muted
                    "
                    onClick={disconnect}
                >

                    Disconnect ArcGIS

                </button>

            </div>


            {children}

        </div>

    );

}