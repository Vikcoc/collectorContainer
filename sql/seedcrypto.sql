CREATE TABLE IF NOT EXISTS new_market_snaps (
    high numeric(18,10) NOT NULL,
    low numeric(18,10) NOT NULL,
    actual numeric(18,10) NOT NULL,
    instrument varchar(20) NOT NULL,
    volume numeric(18,5) NOT NULL,
    usd_volume numeric(18,5) NOT NULL,
    change numeric(18,10) NOT NULL,
    best_bid numeric(18,10) NOT NULL,
    best_bid_size numeric(18,10) NOT NULL,
    best_ask numeric(18,10) NOT NULL,
    best_ask_size numeric(18,10) NOT NULL,
    trade_timestamp numeric(18,0) NOT NULL
);
