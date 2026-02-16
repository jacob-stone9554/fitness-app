import React from "react";
import {cn} from "../lib/cn";

export function Container(props: React.HTMLAttributes<HTMLDivElement>) {
    return (
        <div
            {...props}
            className={cn("mx-auto max-w-6xl px-4", props.className)}
        />
    );
}

export function Section(
    props: React.HTMLAttributes<HTMLElement> & { id?: string }
) {
    return (
        <section
            {...props}
            className={cn("py-16 md:py-20", props.className)}
        />
    );
}

export function Card(props: React.HTMLAttributes<HTMLDivElement>) {
    return (
        <div
            {...props}
            className={cn(
                "rounded-2xl border border-white/10 bg-white/5 hover:bg-white/10 transition-colors",
                props.className
            )}
        />
    );
}

export function Badge(props: React.HTMLAttributes<HTMLDivElement>) {
    return (
        <div
            {...props}
            className={cn(
                "inline-flex items-center gap-2 rounded-full border border-white/10 bg-white/5 px-3 py-1 text-xs text-white/70",
                props.className
            )}
        />
    );
}

type ButtonVariant = "primary" | "secondary" | "ghost";
type ButtonSize = "sm" | "md";

export function ButtonLink({
                               variant = "primary",
                               size = "md",
                               className,
                               ...props
                           }: React.AnchorHTMLAttributes<HTMLAnchorElement> & {
    variant?: ButtonVariant;
    size?: ButtonSize;
}) {
    const base =
        "inline-flex items-center justify-center rounded-xl font-semibold transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-white/30";
    const sizes = size === "sm" ? "px-3 py-2 text-sm" : "px-5 py-3 text-sm";
    const variants: Record<ButtonVariant, string> = {
        primary: "bg-white text-neutral-950 hover:bg-white/90",
        secondary: "border border-white/15 bg-white/5 text-white hover:bg-white/10",
        ghost: "text-white/80 hover:text-white",
    };

    return (
        <a
            {...props}
            className={cn(base, sizes, variants[variant], className)}
        />
    );
}

export function Button({
                           variant = "primary",
                           size = "md",
                           className,
                           type = "button",
                           ...props
                       }: React.ButtonHTMLAttributes<HTMLButtonElement> & {
    variant?: ButtonVariant;
    size?: ButtonSize;
}) {
    const base =
        "inline-flex items-center justify-center rounded-xl font-semibold transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-white/30 disabled:opacity-50 disabled:pointer-events-none";
    const sizes = size === "sm" ? "px-3 py-2 text-sm" : "px-5 py-3 text-sm";
    const variants: Record<ButtonVariant, string> = {
        primary: "bg-white text-neutral-950 hover:bg-white/90",
        secondary: "border border-white/15 bg-white/5 text-white hover:bg-white/10",
        ghost: "text-white/80 hover:text-white",
    };

    return (
        <button
            type={type}
            {...props}
            className={cn(base, sizes, variants[variant], className)}
        />
    );
}

export function Input({
                          className,
                          ...props
                      }: React.InputHTMLAttributes<HTMLInputElement>) {
    return (
        <input
            {...props}
            className={cn(
                "h-11 w-full rounded-xl border border-white/10 bg-white/5 px-3 text-sm text-white placeholder:text-white/40 outline-none",
                "focus:border-white/20 focus:ring-2 focus:ring-white/20",
                className
            )}
        />
    );
}

export function Label({
                          className,
                          ...props
                      }: React.LabelHTMLAttributes<HTMLLabelElement>) {
    return (
        <label
            {...props}
            className={cn("text-sm font-medium text-white/80", className)}
        />
    );
}