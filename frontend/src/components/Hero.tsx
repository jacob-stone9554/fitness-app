import {Badge, ButtonLink, Container} from "../ui/ui.tsx";

function Hero() {
    return (
        <section className="relative overflow-hidden">
            <div className="pointer-events-none absolute inset-0 opacity-40">
                <div className="absolute -top-24 left-1/2 h-72 w-[44rem] -translate-x-1/2 rounded-full bg-white/10 blur-3xl" />
                <div className="absolute -bottom-24 left-1/3 h-72 w-[44rem] -translate-x-1/2 rounded-full bg-white/5 blur-3xl" />
            </div>

            <Container className="pb-16 pt-14 md:pb-24 md:pt-20">
                <div className="max-w-2xl">
                    <Badge>
                        <span className="h-1.5 w-1.5 rounded-full bg-white/60" />
                        Track training, nutrition, and progress in one place
                    </Badge>

                    <h1 className="mt-5 text-4xl font-semibold tracking-tight md:text-6xl">
                        Make consistency your competitive advantage.
                    </h1>

                    <p className="mt-4 text-base leading-relaxed text-white/70 md:text-lg">
                        A simple, fast fitness tracker built for lifters: workouts, progression, and
                        check-ins. Without the clutter.
                    </p>

                    <div className="mt-8 flex flex-col gap-3 sm:flex-row">
                        <ButtonLink href="/dashboard" variant="primary">Start tracking</ButtonLink>
                        <ButtonLink href="#features" variant="secondary">See features</ButtonLink>
                    </div>

                    <p className="mt-4 text-xs text-white/50">Your data is yours. Export anytime. Built for speed.</p>
                </div>
            </Container>
        </section>
    );
}

export default Hero;