function Features() {
    const features = [
        {
            title: "Workout templates",
            desc: "Build routines once. Log sessions in seconds.",
        },
        {
            title: "Progression that’s obvious",
            desc: "See PRs, volume trends, and consistency at a glance.",
        },
        {
            title: "Nutrition check-ins",
            desc: "Quick daily entries. Enough structure to be useful.",
        },
        {
            title: "Export your data",
            desc: "Take your training history with you anytime.",
        },
        {
            title: "Built for lifters",
            desc: "Sets, reps, and RPE. Without a million taps.",
        },
        {
            title: "Secure by design",
            desc: "Clean auth flow with a real backend—no hacks.",
        },
    ];

    return (
        <section id="features" className="mx-auto max-w-6xl px-4 py-16 md:py-20">
            <div className="max-w-2xl">
                <h2 className="text-2xl font-semibold md:text-3xl">Everything you need. Nothing you don’t.</h2>
                <p className="mt-3 text-white/70">
                    The goal is simple: make it easier to show up, log it, and improve.
                </p>
            </div>

            <div className="mt-10 grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
                {features.map((f) => (
                    <div
                        key={f.title}
                        className="rounded-2xl border border-white/10 bg-white/5 p-6 hover:bg-white/10"
                    >
                        <div className="text-base font-semibold">{f.title}</div>
                        <div className="mt-2 text-sm leading-relaxed text-white/65">{f.desc}</div>
                    </div>
                ))}
            </div>
        </section>
    );
}

export default Features;