function HowItWorks() {
    const steps = [
        {
            title: "Create a plan",
            desc: "Pick a program or build your own templates.",
        },
        {
            title: "Log in seconds",
            desc: "Tap sets, adjust weight, save. Done.",
        },
        {
            title: "Review & iterate",
            desc: "See trends, recover better, and push when it counts.",
        },
    ];

    return (
        <section id="how" className="border-t border-white/10">
            <div className="mx-auto max-w-6xl px-4 py-16 md:py-20">
                <h2 className="text-2xl font-semibold md:text-3xl">How it works</h2>
                <div className="mt-10 grid grid-cols-1 gap-4 md:grid-cols-3">
                    {steps.map((s, i) => (
                        <div key={s.title} className="rounded-2xl border border-white/10 bg-white/5 p-6">
                            <div className="text-xs font-semibold text-white/60">Step {i + 1}</div>
                            <div className="mt-2 text-base font-semibold">{s.title}</div>
                            <div className="mt-2 text-sm text-white/65">{s.desc}</div>
                        </div>
                    ))}
                </div>
            </div>
        </section>
    );
}

export default HowItWorks;