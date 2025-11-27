#!/bin/bash

# Portfolio Analyzer Stop Script
# This script stops both the API and Web projects

echo "🛑 Stopping Portfolio Analyzer..."
echo ""

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Read PIDs
if [ -f "$SCRIPT_DIR/.api.pid" ]; then
    API_PID=$(cat "$SCRIPT_DIR/.api.pid")
    echo "Stopping API (PID: $API_PID)..."
    kill $API_PID 2>/dev/null || echo "   API already stopped"
    rm "$SCRIPT_DIR/.api.pid"
fi

if [ -f "$SCRIPT_DIR/.web.pid" ]; then
    WEB_PID=$(cat "$SCRIPT_DIR/.web.pid")
    echo "Stopping Web (PID: $WEB_PID)..."
    kill $WEB_PID 2>/dev/null || echo "   Web already stopped"
    rm "$SCRIPT_DIR/.web.pid"
fi

# Also kill any remaining dotnet processes for these projects
echo ""
echo "Cleaning up any remaining processes..."
pkill -9 -f "PortfolioAnalyzer.Api" 2>/dev/null
pkill -9 -f "PortfolioAnalyzer.Web" 2>/dev/null

# Force kill any process on ports 5000 and 5001
echo "Freeing up ports 5000 and 5001..."
lsof -ti:5000 | xargs kill -9 2>/dev/null || true
lsof -ti:5001 | xargs kill -9 2>/dev/null || true

sleep 1

echo ""
echo "✅ Portfolio Analyzer stopped!"
