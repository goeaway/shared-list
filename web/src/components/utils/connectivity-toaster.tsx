import React, { FC, useState, useEffect } from "react";
import { useToasts } from "react-toast-notifications";

export interface ConnectivityToasterProps {
    onOnline?: () => void;
    onOffline?: () => void;
}

const ConnectivityToaster : FC<ConnectivityToasterProps> = ({onOnline, onOffline}) => {
    const { addToast } = useToasts();

    useEffect(() => {
        const onlineHandler = () => {
            addToast(<span>Connection made. Your changes are being synchronised.</span>, {
                appearance: "info"
            });

            if(onOnline) {
                onOnline();
            }
        }

        const offlineHandler = () => {
            addToast(<span>Connection lost. Changes will not be synchronised.</span>, {
                appearance: "error"
            });

            if(onOffline) {
                onOffline();
            }
        }

        window.addEventListener("online", onlineHandler);
        window.addEventListener("offline", offlineHandler)

        return () => {
            window.removeEventListener("online", onlineHandler);
            window.removeEventListener("offline", offlineHandler);
        }
    });

    return null;
}

export default ConnectivityToaster;