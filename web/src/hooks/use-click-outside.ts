import * as React from "react";

const useClickOutside = (callback: () => void) => {
    const ref = React.useRef();

    React.useEffect(() => {
        const clickHandler = (ev: MouseEvent) => {
            if(!ref.current || (ref.current as any).contains(ev.target)) {
                return;
            }

            callback();
        }

        document.addEventListener("mousedown", clickHandler);
        document.addEventListener("touchstart", clickHandler);

        return () => {
            document.removeEventListener("mousedown", clickHandler);
            document.removeEventListener("touchstart", clickHandler);
        }
    });

    return ref;
};

export default useClickOutside;