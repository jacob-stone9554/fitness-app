import { useState } from "react";
import { register } from "../api/auth";
import { Button, ButtonLink, Card, Container, Input, Label } from "../ui/ui";
import axios from "axios";

export default function RegisterPage() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirm, setConfirm] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);

        if (!email.trim()) return setError("Email is required.");
        if (password.length < 8) return setError("Password must be at least 8 characters.");
        if (password !== confirm) return setError("Passwords do not match.");

        setIsLoading(true);
        try {
            const data = await register({ email, password });

            console.log("Registered:", data);

            alert("Account created! Next we’ll redirect you.");
        } catch (err: unknown) {
            if (axios.isAxiosError(err)) {
                const message =
                    err.response?.data?.message ??
                    err.response?.data ??
                    err.message ??
                    "Registration failed.";

                setError(typeof message === "string" ? message : "Registration failed.");
            } else if (err instanceof Error) {
                setError(err.message);
            } else {
                setError("Registration failed.");
            }
        } finally {
            setIsLoading(false);
        }
    }

    return (
        <div className="min-h-screen bg-neutral-950 text-neutral-50">
            <Container className="py-16">
                <div className="mx-auto max-w-md">
                    <h1 className="text-2xl font-semibold">Create your account</h1>
                    <p className="mt-2 text-sm text-white/60">
                        Start tracking workouts, progression, and check-ins.
                    </p>

                    <Card className="mt-6 p-6">
                        <form onSubmit={onSubmit} className="space-y-4">
                            <div className="space-y-2">
                                <Label htmlFor="email">Email</Label>
                                <Input
                                    id="email"
                                    type="email"
                                    autoComplete="email"
                                    placeholder="you@email.com"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                />
                            </div>

                            <div className="space-y-2">
                                <Label htmlFor="password">Password</Label>
                                <Input
                                    id="password"
                                    type="password"
                                    autoComplete="new-password"
                                    placeholder="At least 8 characters"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                            </div>

                            <div className="space-y-2">
                                <Label htmlFor="confirm">Confirm password</Label>
                                <Input
                                    id="confirm"
                                    type="password"
                                    autoComplete="new-password"
                                    placeholder="Re-enter password"
                                    value={confirm}
                                    onChange={(e) => setConfirm(e.target.value)}
                                />
                            </div>

                            {error && (
                                <div className="rounded-xl border border-red-500/30 bg-red-500/10 px-3 py-2 text-sm text-red-200">
                                    {error}
                                </div>
                            )}

                            <Button type="submit" className="w-full" disabled={isLoading}>
                                {isLoading ? "Creating account..." : "Create account"}
                            </Button>

                            <div className="text-center text-sm text-white/60">
                                Already have an account?{" "}
                                <ButtonLink href="/#/login" variant="ghost" size="sm" className="px-1 py-0">
                                    Log in
                                </ButtonLink>
                            </div>
                        </form>
                    </Card>
                </div>
            </Container>
        </div>
    );
}
