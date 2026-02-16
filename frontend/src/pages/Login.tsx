import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../api/auth";
import { Button, ButtonLink, Card, Container, Input, Label } from "../ui/ui";

export default function LoginPage() {
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    async function onSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);

        if (!email.trim()) return setError("Email is required.");
        if (!password) return setError("Password is required.");

        setIsLoading(true);
        try {
            const data = await login({ email, password });

            console.log("Logged in:", data);

            navigate("/dashboard");
        } catch {
            setError("Login failed.");
        } finally {
            setIsLoading(false);
        }
    }

    return (
        <div className="min-h-screen bg-neutral-950 text-neutral-50">
            <Container className="py-16">
                <div className="mx-auto max-w-md">
                    <h1 className="text-2xl font-semibold">Welcome back</h1>
                    <p className="mt-2 text-sm text-white/60">
                        Log in to continue tracking your training.
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
                                    autoComplete="current-password"
                                    placeholder="Your password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                            </div>

                            {error && (
                                <div className="rounded-xl border border-red-500/30 bg-red-500/10 px-3 py-2 text-sm text-red-200">
                                    {error}
                                </div>
                            )}

                            <Button type="submit" className="w-full" disabled={isLoading}>
                                {isLoading ? "Logging in..." : "Log in"}
                            </Button>

                            <div className="flex items-center justify-between text-sm text-white/60">
                                <ButtonLink href="#" variant="ghost" size="sm" className="px-1 py-0">
                                    Forgot password?
                                </ButtonLink>

                                <span>
                  New here?{" "}
                                    <ButtonLink href="/register" variant="ghost" size="sm" className="px-1 py-0">
                    Create account
                  </ButtonLink>
                </span>
                            </div>
                        </form>
                    </Card>
                </div>
            </Container>
        </div>
    );
}
